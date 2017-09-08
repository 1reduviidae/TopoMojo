using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TopoMojo.Abstractions;
using TopoMojo.Core;
using TopoMojo.Data.Entities;
using TopoMojo.Models;
using TopoMojo.Web;

namespace TopoMojo.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TopologyController : HubController<TopologyHub>
    {
        public TopologyController(
            TopologyManager topologyManager,
            IPodManager podManager,
            IHostingEnvironment env,
            IServiceProvider sp,
            IConnectionManager sigr
        ) : base(sigr, sp)
        {
            _pod = podManager;
            _mgr = topologyManager;
            _env = env;
        }

        private readonly IPodManager _pod;
        private readonly TopologyManager _mgr;
        private readonly IHostingEnvironment _env;

        [HttpPost]
        [JsonExceptionFilter]
        public async Task<Topology> Create([FromBody]Topology topo)
        {
            return await _mgr.Create(topo);
        }

        [HttpPost]
        [JsonExceptionFilter]
        public async Task<Topology> Update([FromBody]Topology topo)
        {
            Topology t = await _mgr.Update(topo);
            Broadcast(topo.GlobalId, new BroadcastEvent<Topology>(HttpContext.User, "TOPO.UPDATED", topo));
            // Actor actor = HttpContext.User.AsActor();
            // Clients.Group(topo.GlobalId).TopoEvent(topo);
            return t;
        }

        [HttpGet("{id}")]
        [JsonExceptionFilterAttribute]
        public async Task<Topology> Load([FromRoute]int id)
        {
            // Topology t = await _mgr.LoadAsync(id);
            // if (t.ShareCode.HasValue())
            //     t.ShareCode = $"{Request.Scheme}://{Request.Host}/enlist/{t.ShareCode}";
            return await _mgr.LoadAsync(id);
        }

        [HttpDelete("{id}")]
        [JsonExceptionFilterAttribute]
        public async Task<bool> Delete([FromRoute]int id)
        {
            Topology topo = await _mgr.LoadAsync(id);
            foreach (Vm vm in await _pod.Find(topo.GlobalId))
                await _pod.Delete(vm.Id);
            Task<bool> task = _mgr.DeleteAsync(topo);
            Broadcast(topo.GlobalId, new BroadcastEvent<Topology>(HttpContext.User, "TOPO.DELETED", topo));
            return await task;
        }

        [HttpPost]
        [JsonExceptionFilter]
        public async Task<SearchResult<TopoSummary>> List([FromBody]Search search)
        {
            List<SearchFilter> filters = search.Filters.ToList();
            filters.Add(new SearchFilter { Name = "published"});
            search.Filters = filters.ToArray();
            return await _mgr.ListAsync(search);
        }

        [HttpPost]
        [JsonExceptionFilter]
        public async Task<SearchResult<TopoSummary>> Mine([FromBody]Search search)
        {
            return await _mgr.ListMine(search);
        }

        [HttpGet("{id}")]
        [JsonExceptionFilter]
        public async Task<Linker[]> Templates([FromRoute]int id)
        {
            return await _mgr.ListTemplates(id);
        }

        [HttpPost]
        [JsonExceptionFilter]
        public async Task<Linker> AddTemplate([FromBody]Linker tref)
        {
            //Broadcast(tref.Topology.GlobalId, new BroadcastEvent<Linker>(HttpContext.User, "TEMPLATE.ADDED", tref));
            return await _mgr.AddTemplate(tref);
        }

        [HttpPut]
        [JsonExceptionFilter]
        public async Task<Linker> UpdateTemplate([FromBody]Linker tref)
        {
            Clients.Group(tref.Topology.GlobalId).TemplateEvent(new BroadcastEvent<Linker>(HttpContext.User, "TEMPLATE.UPDATED", tref));
            return await _mgr.UpdateTemplate(tref);
        }

        [HttpDelete("{id}")]
        [JsonExceptionFilter]
        public async Task<bool> RemoveTemplate([FromRoute]int id)
        {
            return await _mgr.RemoveTemplate(id);
        }

        [HttpPost("{id}")]
        [JsonExceptionFilter]
        public async Task<Linker> CloneTemplate([FromRoute]int id)
        {
            return await _mgr.CloneTemplate(id);
        }

        [HttpGetAttribute("{id}")]
        [JsonExceptionFilterAttribute]
        public async Task<Worker[]> Members([FromRoute] int id)
        {
            return await _mgr.Members(id);
        }

        [HttpGetAttribute("{id}")]
        [JsonExceptionFilterAttribute]
        public async Task<object> Share([FromRoute] int id)
        {
            string code = await _mgr.Share(id, false);
            return new { url = code };
        }

        [HttpGetAttribute("{id}")]
        [JsonExceptionFilterAttribute]
        public async Task<bool> Unshare([FromRoute] int id)
        {
            await _mgr.Share(id, true);
            return true;
        }

        [HttpGetAttribute("{code}")]
        [JsonExceptionFilterAttribute]
        public async Task<bool> Enlist([FromRoute] string code)
        {
            return await _mgr.Enlist(code);
        }

        [HttpDeleteAttribute("{id}/{memberId}")]
        [JsonExceptionFilterAttribute]
        public async Task<bool> Delist([FromRoute] int id, [FromRoute] int memberId)
        {
            return await _mgr.Delist(id, memberId);
        }

        [HttpPutAttribute("{id}")]
        [JsonExceptionFilterAttribute]
        public async Task<bool> Publish([FromRoute] int id)
        {
            return await _mgr.Publish(id, false);
        }

        [HttpPutAttribute("{id}")]
        [JsonExceptionFilterAttribute]
        public async Task<bool> Unpublish([FromRoute] int id)
        {
            return await _mgr.Publish(id, true);
        }

        [HttpGet("{id}")]
        [JsonExceptionFilter]
        public async Task<VmOptions> Isos([FromRoute] string id)
        {
            return await _pod.GetVmIsoOptions(id);
        }

        [HttpGet("{id}")]
        [JsonExceptionFilter]
        public async Task<VmOptions> Nets([FromRoute] string id)
        {
            return await _pod.GetVmNetOptions(id);
        }

    }

}