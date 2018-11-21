import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { User } from '../../../../Models/User';
import {FormControl, Validators} from '@angular/forms';
import {MAT_DIALOG_DEFAULT_OPTIONS} from '@angular/material';
import { OrderHttpService } from '../../../../Services/order-http.service';

@Component({
  selector: 'app-modal-user',
  templateUrl: './modal-user.component.html',
  styleUrls: ['./modal-user.component.css'],
  providers:[ {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: true}}]
  
})
export class ModalUserComponent implements OnInit {

  controller:string;
  model:User;
  OriginalModel : User;
  email = new FormControl('', [Validators.required, Validators.email]);
  constructor(public dialogRef: MatDialogRef<ModalUserComponent>,
    @Inject(MAT_DIALOG_DATA) public data: User,
    private http: OrderHttpService) { }

  ngOnInit() {
    this.model=this.data;
    this.OriginalModel = {...this.data};
    this.controller = "user";
  }

  onNoClick(): void {
    this.dialogRef.close(null);
  }
  
  getErrorMessage() {
    return this.email.hasError('required') ? 'You must enter a value' :
        this.email.hasError('email') ? 'Not a valid email' :
            '';
  }
  saveUSer(){
    // todo call service http to save user
    this.model.isActive = this.model.isActive || false;

    if (! this.model.idUser || this.model.idUser == "" )    {
      this.http.post("https://localhost:44371/api/" + this.controller,this.model,null).subscribe((res:any)=>{
        console.log(res);
      },err=>{
        alert("Error");
        console.log("Error");
      });
    }else{
      console.log("Update");
      this.http.put("https://localhost:44371/api/"+ this.controller +"/" + this.model.idUser, this.model,null).subscribe((res:any)=>{
        alert("OK");
      },err=>{
        alert("Error");
        console.log("Error");
      });
    }
    this.dialogRef.close(this.model);
  }

}
