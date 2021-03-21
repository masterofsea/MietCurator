using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MietCurator.DAL.Contexts.Miet;

namespace MietCurator.Controllers
{
    [ApiController]
    public class MietCuratorController : ControllerBase
    {
        private ILogger<MietCuratorController> Logger { get; }
        private MietContext Context { get; }

        public MietCuratorController(ILogger<MietCuratorController> logger, MietContext context)
        {
            Logger = logger;

            Context = context;
        }


        [HttpGet]
        [Route("LoadGroups")]
        public async Task<IActionResult> LoadGroups()
        {
            try
            {
                Logger.LogInformation("request has been received!");

                var httpClient = new HttpClient();

                var responseStream = await httpClient.GetStreamAsync("https://miet.ru/schedule/groups");


                var fromDbGroups = await Context.Groups.ToListAsync();


                var fromApiGroups = await JsonSerializer.DeserializeAsync<string[]>(responseStream);
                if (fromApiGroups == null) throw new Exception("Loading data from miet API was not successfully done!");

                var newGroups = fromApiGroups.Except(fromDbGroups.Select(i => i.Name))
                    .Select(i => new Groups
                    {
                        Name = i
                    }).ToArray();
                
                Context.AddRange(newGroups);
                
                await Context.SaveChangesAsync();

                return new ObjectResult($"Loaded {newGroups.Length} new groups");
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Exception occured while request was handling {e.Message}");

                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpGet]
        [Route("GetGroups")]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                Logger.LogInformation("request has been received!");

                var groups = await Context.Groups.ToListAsync();

                return new ObjectResult(groups);
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Exception occured while request was handling {e.Message}");

                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}