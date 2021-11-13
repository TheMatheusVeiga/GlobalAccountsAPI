using System;
using System.Collections.Generic;
using System.Data;
using dotnet_core_api.Models;
using dotnet_core_api.Utils;

namespace dotnet_core_api.Business
{
    public class GroupBusiness
    {
        public Result Add(Group group)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"INSERT INTO public.groups(name) VALUES('{group.name}');";

                DataSet dataResult = postgre.ExecuteQuery(sql);

                if (dataResult.HasErrors)
                    return Result.CreateResult(true, "Erro ao criar grupo.");

                return Result.CreateResult(false, "Grupo criado com sucesso!", null);

            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }

        }

        public Result GetGroups()
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"Select * from groups";
                DataSet dataResult = postgre.ExecuteQuery(sql);
                if (dataResult.Tables.Count <= 0)
                    return Result.CreateResult(true, "Nenhum grupo encontrado.");

                List<Group> groups = new List<Group>();
                foreach (DataRow row in dataResult.Tables[0].Rows)
                {
                    var group = new Group();
                    group.idGroup = ConvertDatasetToClass.validateValue(row, "idGroup", 0);
                    group.name = ConvertDatasetToClass.validateValue(row, "name", "");
                    groups.Add(group);
                }

                return Result.CreateResult(false, "Grupos listados com sucesso!", groups);
            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }

        public Result GetGroupTools(string nameGroup)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"SELECT grp.\"idGroup\", grp.name AS nameGroup, tls.\"idTool\", tls.name AS nameTool FROM public.groups grp " +
                          $"INNER JOIN public.group_tools gt ON (gt.\"idGroup\" = grp.\"idGroup\") " +
                          $"INNER JOIN public.tools tls ON (tls.\"idTool\" = gt.\"idTool\") " +
                          $"WHERE grp.name = '{nameGroup}'";

                DataSet dataResult = postgre.ExecuteQuery(sql);
                if (dataResult.Tables.Count <= 0)
                    return Result.CreateResult(true, "Nenhuma ferramenta atribuida ao grupo.");


                List<GroupTools> groupTools = new List<GroupTools>();
                foreach (DataRow row in dataResult.Tables[0].Rows)
                {
                    var groupTool = new GroupTools();
                    groupTool.idGroup = ConvertDatasetToClass.validateValue(row, "idGroup", 0);
                    groupTool.nameGroup = ConvertDatasetToClass.validateValue(row, "nameGroup", "");
                    groupTool.idTool = ConvertDatasetToClass.validateValue(row, "idTool", 0);
                    groupTool.nameTool = ConvertDatasetToClass.validateValue(row, "nameTool", "");
                    groupTools.Add(groupTool);
                }

                return Result.CreateResult(false, "Relação de ferramentas por grupo listado com sucesso!", groupTools);
            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }

        public Result Assign(IEnumerable<AssignmentObjectGroupUser> list)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();
                var sql = "";

                foreach (AssignmentObjectGroupUser item in list)
                {
                    sql += $"INSERT INTO public.user_groups(\"idGroup\", \"idUser\") VALUES( (SELECT \"idGroup\" FROM public.groups WHERE name = '{item.nameGroup}'), (SELECT \"idUser\" FROM public.users WHERE email = '{item.emailUser}') ); ";
                }

                DataSet dataResult = postgre.ExecuteQuery(sql);

                if (dataResult.HasErrors)
                    return Result.CreateResult(true, "Erro ao assimilar grupos aos usuários.");

                return Result.CreateResult(false, "Grupos assimilados aos usuários com sucesso!", null);

            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }
    }
}
