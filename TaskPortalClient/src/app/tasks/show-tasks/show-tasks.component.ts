import { Component, OnInit } from '@angular/core';
import {SharedService} from 'src/app/shared.service';

@Component({
  selector: 'app-show-tasks',
  templateUrl: './show-tasks.component.html',
  styleUrls: ['./show-tasks.component.css']
})
export class ShowTasksComponent implements OnInit {

  constructor(private service:SharedService) { }

  TasksList:any=[];

  ModalTitle: string;
  ActivateAddUpdateTaskComponent: boolean=false;
  task: any;

  addClick(){
    this.task= {
      partitionKey:"",
      rowKey:0,
      name:"",
      description:"",
      isComplete:""
    }
    this.ModalTitle="Add Task";
    this.ActivateAddUpdateTaskComponent=true;

  }

  updateClick(item: any){
    this.task= {
      partitionKey:item.partitionKey,
      rowKey:item.rowKey,
      name:item.name,
      description:item.description,
      isComplete:item.isComplete}
    this.ModalTitle="Update Task info";
    this.ActivateAddUpdateTaskComponent=true;
  }

  deleteClick(item: { rowKey: any; }){
    if(confirm('Are you sure??')){
      this.service.deleteTask(item.rowKey).subscribe(data=>{
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
    });

  }
}

