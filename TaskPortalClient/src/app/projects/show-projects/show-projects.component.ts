import { Component, OnInit } from '@angular/core';
import {SharedService} from "../../shared.service";


@Component({
  selector: 'app-show-projects',
  templateUrl: './show-projects.component.html',
  styleUrls: ['./show-projects.component.css']
})
export class ShowProjectsComponent implements OnInit {

  constructor(private service:SharedService) { }

  ProjectList:any=[];

  ModalTitle: string;
  ActivateAddUpdateProjectComponent: boolean=false;
  project: any;

  ngOnInit(): void {
    this.refreshProjectsList();
  }

  addClick(){
    this.project= {
      rowKey:0,
      partitionKey: "",
      description: "",
      code: ""
    }
    console.log(this.project.rowKey)
    this.ModalTitle="Add Project";
    this.ActivateAddUpdateProjectComponent=true;
    console.log('end of add click flow');
  }

  closeClick(){
    this.ActivateAddUpdateProjectComponent=false;
    this.refreshProjectsList();
  }

  updateClick(item: any){
    console.log('hello from update component')
    this.project= {
      rowKey:item.rowKey,
      partitionKey:item.partitionKey,
      description:item.description,
      code:item.code}
    this.ModalTitle="Update Project info";
    this.ActivateAddUpdateProjectComponent=true;
  }

  deleteClick(item: { rowKey: any; }){
    if(confirm('Are you sure??')){
      this.service.deleteProject(item.rowKey).subscribe(data=>{
        alert("Record deleted!");
        this.refreshProjectsList();
      })
    }
  }

  refreshProjectsList(){
    this.service.getProjectsList().subscribe(data=>{
      this.ProjectList = data;
    });
  }
}
