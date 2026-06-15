import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {User} from '../../types/user';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getUserWithRoles(){
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }
  updateUserRoles(userId:string, roles: string[]){
    return this.http.post<string[]>(this.baseUrl + 'admin/edit-roles/' + userId + '?roles=' + roles, {})
  }
}
