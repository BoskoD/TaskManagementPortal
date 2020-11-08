import { Component, OnInit } from '@angular/core';
import {SharedService} from 'src/app/shared.service';


@Component({
  selector: 'app-show-projects',
  templateUrl: './show-projects.component.html',
  styleUrls: ['./show-projects.component.css']
})
export class ShowProjectsComponent implements OnInit {

  constructor(private service:SharedService) { }

  ProjectsList:any=[];

  ModalTitle: string;
  ActivateAddUpdateProjectComponent: boolean=false;
  proj: any;

  addClick(){
    this.proj= {
      Id:0,
      Name:"",
      Description:"",
      Code:""
    }
    this.ModalTitle="Add Project";
    this.ActivateAddUpdateProjectComponent=true;

  }

  updateClick(item: any){
    this.proj= {
      Id:item.id,
      Name:item.Name,
      Description:item.Description,
      Code:item.Code}
    this.ModalTitle="Update Project info";
    this.ActivateAddUpdateProjectComponent=true;
  }

  deleteClick(item: { id: any; }){
    if(confirm('Are you sure??')){
      this.service.deleteProject(item.id).subscribe(data=>{
        alert("Record deleted!");
        this.refreshProjectsList();
      })
    }
  }


  closeClick(){

    this.ActivateAddUpdateProjectComponent=false;
    this.refreshProjectsList();
  }


  ngOnInit(): void {
    this.refreshProjectsList();
  }



  refreshProjectsList(){
    this.service.getProjectsList().subscribe(data=>{
      this.ProjectsList = data;
    });

  }




}
