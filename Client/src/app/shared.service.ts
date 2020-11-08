import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  readonly ApiUrl = 'https://localhost:44351/';
  constructor(private http:HttpClient) { }

  getProjectsList():Observable<any[]>{
    return this.http.get<any>(this.ApiUrl+'Project/readall');
  }

  addProject(val:any){
    return this.http.post(this.ApiUrl+'Project/create',val);
  }

  updateProject(val:any){
    return this.http.put(this.ApiUrl+'Project/update/',val);
  }

  deleteProject(val:any){
    return this.http.delete(this.ApiUrl+'Project/delete/'+val);
  }

  getAllTasksInAProject(val: any){
    return this.http.get(this.ApiUrl+'Project/tasksbyproject/{id}/'+val);
  }



  getTasksList():Observable<any[]>{
    return this.http.get<any>(this.ApiUrl+'Task/readall');
  }

  addTask(val:any){
    return this.http.post(this.ApiUrl+'Task/create',val);
  }

  updateTask(val:any){
    return this.http.put(this.ApiUrl+'Task/update/',val);
  }

  deleteTask(val:any){
    return this.http.delete(this.ApiUrl+'Task/delete/'+val);
  }

  getAllProjectNames():Observable<any[]>{
    return this.http.get<any[]>(this.ApiUrl+'Task/getallprojectnames');
  }
}
