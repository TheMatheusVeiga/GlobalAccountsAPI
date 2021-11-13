using System;
using System.Collections.Generic;
using System.Data;
using dotnet_core_api.Models;
using dotnet_core_api.Utils;

namespace dotnet_core_api.Business
{
    public class ToolBusiness
    {
        public Result Add(Tool tool)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"INSERT INTO public.tools(name) VALUES('{tool.name}');";

                DataSet dataResult = postgre.ExecuteQuery(sql);

                if (dataResult.HasErrors)
                    return Result.CreateResult(true, "Erro ao criar Ferramenta.");

                return Result.CreateResult(false, "Ferramenta criada com sucesso!", null);

            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }

        }

        public Result GetTools()
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"Select * from tools";
                DataSet dataResult = postgre.ExecuteQuery(sql);
                if (dataResult.Tables.Count <= 0)
                    return Result.CreateResult(true, "Nenhuma ferramenta encontrada.");

                List<Tool> tools = new List<Tool>();
                foreach (DataRow row in dataResult.Tables[0].Rows)
                {
                    var tool = new Tool();
                    tool.idTool = ConvertDatasetToClass.validateValue(row, "idTool", 0);
                    tool.name = ConvertDatasetToClass.validateValue(row, "name", "");
                    tools.Add(tool);
                }

                return Result.CreateResult(false, "Ferramentas listadas com sucesso!", tools);
            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }

        public Result Assign(IEnumerable<AssignmentObjectToolGroup> list)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();
                var sql = "";

                foreach (AssignmentObjectToolGroup item in list)
                {
                    sql += $"INSERT INTO public.group_tools(\"idGroup\", \"idTool\") VALUES( (SELECT \"idGroup\" FROM public.groups WHERE name = '{item.nameGroup}'), (SELECT \"idTool\" FROM public.tools WHERE name = '{item.nameTool}') ); ";
                }

                DataSet dataResult = postgre.ExecuteQuery(sql);

                if (dataResult.HasErrors)
                    return Result.CreateResult(true, "Erro ao assimilar as ferramentas aos grupos.");

                return Result.CreateResult(false, "Ferramentas assimiladas aos grupos com sucesso!", null);

            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }
    }
}
