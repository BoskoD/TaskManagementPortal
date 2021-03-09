import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { baseUrl} from "../environments/environment"

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(private http:HttpClient) { }

  getProjectsList():Observable<any[]>{
    return this.http.get<any>(`${baseUrl}projects`);
  }

  addProject(val:any){
    return this.http.post(`${baseUrl}project`, val);
  }

  updateProject(val:any){
    return this.http.put(`${baseUrl}project/${val.id}`, val);
  }

  deleteProject(val:any){
    return this.http.delete(`${baseUrl}project/` + val);
  }

  getTasksList():Observable<any[]>{
    return this.http.get<any>(`${baseUrl}tasks`);
  }

  addTask(val:any){
    return this.http.post(`${baseUrl}task`, val);
  }

  updateTask(val:any){
    return this.http.put(`${baseUrl}task/${val.id}`, val);
  }

  deleteTask(val:any){
    return this.http.delete(`${baseUrl}task/` + val);
  }
}
