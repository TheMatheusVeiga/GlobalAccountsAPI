using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_core_api.Business;
using dotnet_core_api.Models;
using dotnet_core_api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_core_api.Controllers
{
    [Route("api/tool")]
    public class ToolController: Controller
    {
        [HttpPost("add")]
        public IActionResult Add([FromBody] Tool tool)
        {
            if (string.IsNullOrWhiteSpace(tool.name))
                return BadRequest(Result.CreateResult(true, "Nome não pode ser vazio"));
            Result result = Result.GetInstance;

            ToolBusiness toolBusiness = new ToolBusiness();
            result = toolBusiness.Add(tool);

            return Ok(result);
        }

        [HttpGet("gettools")]
        public IActionResult GetTools()
        {
            Result result = Result.GetInstance;

            ToolBusiness toolBusiness = new ToolBusiness();
            result = toolBusiness.GetTools();

            return Ok(result);
        }

        [HttpPost("assign")]
        public IActionResult Assign([FromBody] IEnumerable<AssignmentObjectToolGroup> list)
        {

            foreach (AssignmentObjectToolGroup item in list)
            {
                if (string.IsNullOrWhiteSpace(item.nameGroup))
                    return BadRequest(Result.CreateResult(true, "Um ou mais itens sem nome de grupo."));
                if (string.IsNullOrWhiteSpace(item.nameTool))
                    return BadRequest(Result.CreateResult(true, "Um ou mais itens sem nome de ferramenta."));
            }
            Result result = Result.GetInstance;

            ToolBusiness toolBusiness = new ToolBusiness();
            result = toolBusiness.Assign(list);

            return Ok(result);
        }

    }
}
