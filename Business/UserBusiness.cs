using System;
using System.Collections.Generic;
using System.Data;
using dotnet_core_api.Models;
using dotnet_core_api.Utils;

namespace dotnet_core_api
{
    public class UserBusiness
    {
        public Result ValidateLogin(string email, string password)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();
                var sql = $"Select * from users where email = '{email}' ";
                DataSet dataResult = postgre.ExecuteQuery(sql);
                if (dataResult.Tables.Count == 0)
                    return Result.CreateResult(true, "Email não encontrado.");
                var row = dataResult.Tables[0].Rows[0];
                var user = new User();
                user.idUser = ConvertDatasetToClass.validateValue(row, "idUser", 0);
                user.name = ConvertDatasetToClass.validateValue(row, "name", "");
                user.email = ConvertDatasetToClass.validateValue(row, "email", "");
                user.password = ConvertDatasetToClass.validateValue(row, "password", "");

                if (user.password != password)
                    return Result.CreateResult(true, "Senha incorreta");

                return Result.CreateResult(false, "Login efetuado com sucesso!", null);

            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }

        }

        public Result Register(string email, string password, string name)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"INSERT INTO public.users(email, name, password) VALUES('{email}', '{name}', '{password}');";

                DataSet dataResult = postgre.ExecuteQuery(sql);

                if (dataResult.HasErrors)
                    return Result.CreateResult(true, "Erro ao registrar usuário.");

                return Result.CreateResult(false, "Usuário registrado com sucesso!", null);

            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }

        }

        public Result GetUsers()
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"Select * from users";
                DataSet dataResult = postgre.ExecuteQuery(sql);
                if (dataResult.Tables.Count <= 0)
                    return Result.CreateResult(true, "Nenhum usuário encontrado.");

                List<User> users = new List<User>();
                foreach (DataRow row in dataResult.Tables[0].Rows)
                {
                    var user = new User();
                    user.idUser = ConvertDatasetToClass.validateValue(row, "idUser", 0);
                    user.name = ConvertDatasetToClass.validateValue(row, "name", "");
                    user.email = ConvertDatasetToClass.validateValue(row, "email", "");
                    user.password = ConvertDatasetToClass.validateValue(row, "password", "");
                    users.Add(user);
                }

                return Result.CreateResult(false, "Usuários listados com sucesso!", users);
            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }

        public Result GetUserGroups(string email)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"SELECT usr.\"idUser\", usr.email, usr.name AS nameUser, grp.* FROM public.users usr " +
                          $"INNER JOIN public.user_groups ug ON (ug.\"idUser\" = usr.\"idUser\") " +
                          $"INNER JOIN public.groups grp ON (grp.\"idGroup\" = ug.\"idGroup\") " +
                          $"WHERE usr.email = '{email}'";

                DataSet dataResult = postgre.ExecuteQuery(sql);
                if (dataResult.Tables.Count <= 0)
                    return Result.CreateResult(true, "Nenhum grupo atribuido ao usuário.");


                List<UserGroups> userGroups = new List<UserGroups>();
                foreach (DataRow row in dataResult.Tables[0].Rows)
                {
                    var userGroup = new UserGroups();
                    userGroup.idUser = ConvertDatasetToClass.validateValue(row, "idUser", 0);
                    userGroup.nameUser = ConvertDatasetToClass.validateValue(row, "nameUser", "");
                    userGroup.email = ConvertDatasetToClass.validateValue(row, "email", "");
                    userGroup.idGroup = ConvertDatasetToClass.validateValue(row, "idGroup", 0);
                    userGroup.nameGroup = ConvertDatasetToClass.validateValue(row, "name", "");
                    userGroups.Add(userGroup);
                }

                return Result.CreateResult(false, "Relação de grupos por usuário listado com sucesso!", userGroups);
            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }

        public Result GetUserTools(string email)
        {
            try
            {
                Result result = Result.GetInstance;
                PostgreConnection postgre = new PostgreConnection();

                var sql = $"SELECT DISTINCT usr.\"idUser\", usr.email, usr.name AS nameUser, tls.* FROM public.users usr " +
                          $"INNER JOIN public.user_groups ug ON (ug.\"idUser\" = usr.\"idUser\") " +
                          $"INNER JOIN public.groups grp ON (grp.\"idGroup\" = ug.\"idGroup\") " +
                          $"INNER JOIN public.group_tools gt ON (gt.\"idGroup\" = grp.\"idGroup\") " +
                          $"INNER JOIN public.tools tls ON (tls.\"idTool\" = gt.\"idTool\") " +
                          $"WHERE usr.email = '{email}' " +
                          $"ORDER BY usr.\"idUser\"";

                DataSet dataResult = postgre.ExecuteQuery(sql);
                if (dataResult.Tables.Count <= 0)
                    return Result.CreateResult(true, "Nenhuma ferramenta atribuida ao usuário.");


                List<UserTools> userTools = new List<UserTools>();
                foreach (DataRow row in dataResult.Tables[0].Rows)
                {
                    var userTool = new UserTools();
                    userTool.idUser = ConvertDatasetToClass.validateValue(row, "idUser", 0);
                    userTool.nameUser = ConvertDatasetToClass.validateValue(row, "nameUser", "");
                    userTool.email = ConvertDatasetToClass.validateValue(row, "email", "");
                    userTool.idTool = ConvertDatasetToClass.validateValue(row, "idTool", 0);
                    userTool.nameGroup = ConvertDatasetToClass.validateValue(row, "name", "");
                    userTools.Add(userTool);
                }

                return Result.CreateResult(false, "Relação de ferramentas por usuário listado com sucesso!", userTools);
            }
            catch (Exception ex)
            {
                return Result.CreateResult(true, ex.Message);
            }
        }
    }
}