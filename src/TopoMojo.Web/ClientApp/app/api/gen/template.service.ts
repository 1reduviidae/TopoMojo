
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Rx';
import { GeneratedService } from "./_service";
import { ChangedTemplate,Search,Template,TemplateDetail,TemplateSummary,TemplateSummarySearchResult } from "./models";

@Injectable()
export class GeneratedTemplateService extends GeneratedService {

    constructor(
       protected http: HttpClient
    ) { super(http); }

	public getTemplates(search: Search) : Observable<TemplateSummarySearchResult> {
		return this.http.get<TemplateSummarySearchResult>("/api/templates" + this.paramify(search));
	}
	public getTemplate(id: number) : Observable<Template> {
		return this.http.get<Template>("/api/template/" + id);
	}
	public deleteTemplate(id: number) : Observable<boolean> {
		return this.http.delete<boolean>("/api/template/" + id);
	}
	public detailedTemplate(id: number) : Observable<TemplateDetail> {
		return this.http.get<TemplateDetail>("/api/template/" + id + "/detailed");
	}
	public createTemplate(model: TemplateDetail) : Observable<TemplateDetail> {
		return this.http.post<TemplateDetail>("/api/template/create", model);
	}
	public configureTemplate(template: TemplateDetail) : Observable<TemplateDetail> {
		return this.http.put<TemplateDetail>("/api/template/configure", template);
	}
	public linkTemplate(id: number, topoId: number) : Observable<Template> {
		return this.http.get<Template>("/api/template/" + id + "/link/" + topoId);
	}
	public unlinkTemplate(id: number) : Observable<Template> {
		return this.http.get<Template>("/api/template/" + id + "/unlink");
	}
	public putTemplate(template: ChangedTemplate) : Observable<Template> {
		return this.http.put<Template>("/api/template", template);
	}

}

