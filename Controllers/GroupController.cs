using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_core_api.Business;
using dotnet_core_api.Models;
using dotnet_core_api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_core_api.Controllers
{
    [Route("api/group")]
    public class GroupController : Controller
    {
        [HttpPost("add")]
        public IActionResult Add([FromBody] Group group)
        {
            if (string.IsNullOrWhiteSpace(group.name))
                return BadRequest(Result.CreateResult(true, "Nome não pode ser vazio"));
            Result result = Result.GetInstance;

            GroupBusiness groupBusiness = new GroupBusiness();
            result = groupBusiness.Add(group);

            return Ok(result);
        }

        [HttpGet("getgroups")]
        public IActionResult GetGroups()
        {
            Result result = Result.GetInstance;

            GroupBusiness groupBusiness = new GroupBusiness();
            result = groupBusiness.GetGroups();

            return Ok(result);
        }

        [HttpGet("getgrouptools")]
        public IActionResult GetGroupTools(string nameGroup)
        {
            Result result = Result.GetInstance;

            if (string.IsNullOrWhiteSpace(nameGroup))
                return BadRequest(Result.CreateResult(true, "Grupo não pode ser vazio"));

            GroupBusiness groupBusiness = new GroupBusiness();
            result = groupBusiness.GetGroupTools(nameGroup);

            return Ok(result);
        }

        [HttpPost("assign")]
        public IActionResult Assign([FromBody] IEnumerable<AssignmentObjectGroupUser> list)
        {

            foreach (AssignmentObjectGroupUser item in list)
            {
                if(string.IsNullOrWhiteSpace(item.nameGroup))
                    return BadRequest(Result.CreateResult(true, "Um ou mais itens sem nome de grupo."));
                if (string.IsNullOrWhiteSpace(item.emailUser))
                    return BadRequest(Result.CreateResult(true, "Um ou mais itens sem nome de usuário."));
            }
            Result result = Result.GetInstance;

            GroupBusiness groupBusiness = new GroupBusiness();
            result = groupBusiness.Assign(list);

            return Ok(result);
        }
    }
}
