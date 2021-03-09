import { Component, OnInit } from '@angular/core';
import {SharedService} from 'src/app/shared.service';
import {AddUpdateTaskComponent} from "../add-update-task/add-update-task.component"

@Component({
  selector: 'app-show-tasks',
  templateUrl: './show-tasks.component.html',
  styleUrls: ['./show-tasks.component.css']
})
export class ShowTasksComponent implements OnInit {

  constructor(private service:SharedService) { }

  TasksList:any=[];
  ProjectsList:any=[];

  ModalTitle: string;
  ActivateAddUpdateTaskComponent: boolean=false;
  task: any;

  loadProjectsList(){
    this.service.getProjectsList().subscribe((data:any)=>{
      this.ProjectsList=data;

      projectId: data.id;

      console.log(data);
    });
  }


  addClick(){ 
    this.task= {
      projectId:"",
      id:0,
      name:"",
      description:"",
      isComplete:""
    }
    this.ModalTitle="Add Task";
    this.ActivateAddUpdateTaskComponent=true;

  }

  updateClick(item: any){
    this.task= {
      projectId:item.projectId,
      id:item.id,
      name:item.name,
      description:item.description,
      isComplete:item.isComplete}
    this.ModalTitle="Update Task info";
    this.ActivateAddUpdateTaskComponent=true;
    console.log(item)

  }

  deleteClick(item: { id: any; }){
    if(confirm('Are you sure??')){
      this.service.deleteTask(item.id).subscribe(data=>{
        alert("Record deleted!");
        this.refreshTasksList();
      })
    }
  }


  closeClick(){

    this.ActivateAddUpdateTaskComponent=false;
    this.refreshTasksList();
  }

  ngOnInit(): void {
    this.refreshTasksList();
  }

  refreshTasksList(){
    this.service.getTasksList().subscribe(data=>{
      this.TasksList = data;
      this.loadProjectsList();
    });

  }
}

