using System;
using System.IO;
using System.Linq;
using WindevReader;

namespace Hirondelle.Exporters;

public static class EFAnalysisExportManager
{
    public static void Export(WddWindevAnalysis analysis, string targetFolder)
    {
        string projectName = HirondelleManager.CurrentProject.Name.Replace(" ", "");

        Directory.CreateDirectory($"{targetFolder}/Models");
        
        BuildDbContext(analysis, $"{targetFolder}/{projectName}DbContext.cs");
        BuildModels(analysis, $"{targetFolder}/Models");
    }

    private static void BuildDbContext(WddWindevAnalysis analysis, string filename)
    {
        string projectName = HirondelleManager.CurrentProject.Name.Replace(" ", "");

        string dbSetsList = BuildDbSetList(analysis);
        
        string file = $$"""
                        using Microsoft.EntityFrameworkCore;
                        
                        namespace {{projectName}};
                        
                        public class {{projectName}}DbContext : DbContext 
                        {
                        {{dbSetsList}}
                        }
                        """;
        
        File.WriteAllText(filename, file);
    }

    private static string BuildDbSetList(WddWindevAnalysis analysis)
    {
        string list = "";
        
        foreach (WindevDataTable table in analysis.Tables)
        {
            list += $"    public DbSet<{table.Name}_Model> {table.Name} {{ get; set; }}" + Environment.NewLine;
        }

        return list;
    }

    private static void BuildModels(WddWindevAnalysis analysis, string path)
    {
        string projectName = HirondelleManager.CurrentProject.Name.Replace(" ", "");
        
        foreach (WindevDataTable table in analysis.Tables)
        {
            string className = $"{table.Name}_Model";
            
            string filename = $"{path}/{className}.cs";

            string attribList = BuildAttributeList(table);
            string fieldList = BuildFieldList(table, analysis);
            
            string file = $$"""
                            using Microsoft.EntityFrameworkCore;

                            namespace {{projectName}};

                            /*
                                Fichier {{table.Name}} : {{table.Caption}}
                                Nom physique : {{table.PhysicalName}}
                            */
                            {{attribList}}
                            public class {{table.Name}}_Model
                            {
                            {{fieldList}}
                            }
                            """;
            
            File.WriteAllText(filename, file);
        }
    }

    private static string BuildAttributeList(WindevDataTable table)
    {
        string list = "";

        foreach (WindevDataField field in table.Fields)
        {
            if (field.IsCompositeKey)
            {
                list += $"[Index({string.Join(", ", field.KeyComponents.Select(k => 
                    $"nameof({k.FieldName})"))}, Name = \"{field.Name}\")]" + Environment.NewLine;
            }
        }

        return list.TrimEnd(Environment.NewLine).ToString();
    }

    private static string BuildFieldList(WindevDataTable table, WddWindevAnalysis analysis)
    {
        string list = "";
        
        foreach (WindevDataField field in table.Fields)
        {
            list += "    ";

            if (!string.IsNullOrWhiteSpace(field.Caption))
            {
                list += "// " + field.Caption + Environment.NewLine + "    ";
            }
            
            if (field.IsPrimaryKey ?? false) list += "[Key] ";
            
            list += $"public {MapFieldTypeToCSharp(field.RawTypeCode)} {field.Name} {{ get; set; }}" + Environment.NewLine;
        }

        return list;
    }

    private static string MapFieldTypeToCSharp(ushort fieldRawTypeCode)
    {
        return fieldRawTypeCode switch
        {
            2 => "string",
            5 => "int",
            11 => "DateTime",
            14 => "DateTime",
            24 => "ulong",
            25 => "long",
            26 => "uint",
            27 => "ulong",
            28 => "byte[]",
            29 => "string",
            34 => "DateTime",
            36 => "byte",
            37 => "bool",
            38 => "uint",
            _ => "object"
        };
    }
}