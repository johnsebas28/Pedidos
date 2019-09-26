import { Component, OnInit } from '@angular/core';
import { UserLogin } from '../../Models/UserLogin';
import { OrderHttpService } from '../../Services/order-http.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  
  public userLogin: UserLogin;
  public status:string;
  endPoint: string;

  constructor(private _OrderHttpService: OrderHttpService) { 
    this.status = '';
    this.userLogin = new UserLogin();
    this.endPoint = 'https://localhost:44371/api/User/user-login';
  }

  ngOnInit() {
    console.log("Componente Login creado!");

  }

  onSubmit(){
     
     this._OrderHttpService.post(this.endPoint,this.userLogin,null).subscribe(
       response=>{
        console.log(response);
       },
       error=>{
         console.log("Error en login");
        console.log(error);
       }
     );
  }

}
