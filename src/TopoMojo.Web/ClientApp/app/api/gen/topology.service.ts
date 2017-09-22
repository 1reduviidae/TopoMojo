
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Rx';
import { GeneratedService } from "./_service";
import { TopologySummarySearchResult,Search,TopologySummary,ChangedTopology,Topology,Worker,Template,NewTopology,TopologyState,VmOptions } from "./models";

@Injectable()
export class GeneratedTopologyService extends GeneratedService {

    constructor(
       protected http: HttpClient
    ) { super(http); }

	public getTopologies(search : Search) : Observable<TopologySummarySearchResult> {
		return this.http.get<TopologySummarySearchResult>("/api/topologies" + this.queryStringify(search));
	}
	public putTopology(model: ChangedTopology) : Observable<Topology> {
		return this.http.put<Topology>("/api/topology", model);
	}
	public postTopology(model: NewTopology) : Observable<Topology> {
		return this.http.post<Topology>("/api/topology", model);
	}
	public getTopology(id: number) : Observable<Topology> {
		return this.http.get<Topology>("/api/topology/" + id);
	}
	public deleteTopology(id: number) : Observable<boolean> {
		return this.http.delete<boolean>("/api/topology/" + id);
	}
	public publishTopology(id: number) : Observable<TopologyState> {
		return this.http.get<TopologyState>("/api/topology/" + id + "/publish");
	}
	public unpublishTopology(id: number) : Observable<TopologyState> {
		return this.http.get<TopologyState>("/api/topology/" + id + "/unpublish");
	}
	public shareTopology(id: number) : Observable<TopologyState> {
		return this.http.get<TopologyState>("/api/topology/" + id + "/share");
	}
	public unshareTopology(id: number) : Observable<TopologyState> {
		return this.http.get<TopologyState>("/api/topology/" + id + "/unshare");
	}
	public enlistWorker(code: string) : Observable<boolean> {
		return this.http.get<boolean>("/api/worker/enlist/" + code);
	}
	public delistWorker(workerId: number) : Observable<boolean> {
		return this.http.delete<boolean>("/api/worker/delist/" + workerId);
	}
	public isosTopology(id: string) : Observable<VmOptions> {
		return this.http.get<VmOptions>("/api/topology/" + id + "/isos");
	}
	public netsTopology(id: string) : Observable<VmOptions> {
		return this.http.get<VmOptions>("/api/topology/" + id + "/nets");
	}

}

