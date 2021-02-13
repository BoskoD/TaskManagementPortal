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
    return this.http.get<any>(`${baseUrl}project/readall`);
  }

  addProject(val:any){
    return this.http.post(`${baseUrl}project/create`, val);
  }

  updateProject(val:any){
    return this.http.put(`${baseUrl}project/update`, val);
  }

  deleteProject(val:any){
    return this.http.delete(`${baseUrl}project/delete/` + val);
  }

  getAllTasksInAProject(val: any){
    return this.http.get(`${baseUrl}project/taskbyproject/{id}`, val);
  }

  getTasksList():Observable<any[]>{
    return this.http.get<any>(`${baseUrl}task/readall`);
  }

  addTask(val:any){
    return this.http.post(`${baseUrl}task/create`, val);
  }

  updateTask(val:any){
    return this.http.put(`${baseUrl}task/update`, val);
  }

  deleteTask(val:any){
    return this.http.delete(`${baseUrl}task/delete/` + val);
  }

  readAllProjectNames():Observable<any[]>{
    return this.http.get<any[]>(`${baseUrl}task/readallprojectnames`);
  }
}
