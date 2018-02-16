
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Rx';
import { GeneratedService } from "./_service";
import { ChangedProfile,Profile,ProfileSearchResult,Search } from "./models";

@Injectable()
export class GeneratedProfileService extends GeneratedService {

    constructor(
       protected http: HttpClient
    ) { super(http); }

	public getProfiles(search: Search) : Observable<ProfileSearchResult> {
		return this.http.get<ProfileSearchResult>("/api/profiles" + this.paramify(search));
	}
	public postProfile(profile: ChangedProfile) : Observable<ChangedProfile> {
		return this.http.post<ChangedProfile>("/api/profile", profile);
	}

}

