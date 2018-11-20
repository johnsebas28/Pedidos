import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup} from '@angular/forms';
import { NotificationsService } from './Services/notifications.service';
import {MatSnackBar} from '@angular/material';
import { NotificationAlertComponent } from './Components/Common/notification-alert/notification-alert.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers:[NotificationsService]
})
export class AppComponent implements OnInit {
  title = 'orders';
  urlPedidos = 'assets/images/PedidoLogo.png';
  options: FormGroup; 
  PendingOrders:number;
  countdownEndRef: any;
  constructor(private fb: FormBuilder, 
    private notificationsService: NotificationsService,
    public snackBar: MatSnackBar ){
    this.options = fb.group({
      bottom: 0,
      fixed: false,
      top: 0
    });
    this.PendingOrders = 0;
  }


  ngOnInit(){
    this.countdownEndRef = this.notificationsService.onChangePendingOrders$.subscribe((res)=>{
      this.PendingOrders = res;
      this.snackBar.openFromComponent(NotificationAlertComponent, {
        duration: 3000,
      });
  });
   //shouldRun = [/(^|\.)plnkr\.co$/, /(^|\.)stackblitz\.io$/].some(h => h.test(window.location.host));
  //shouldRun = true;
}

testeppp(){
  alert("hola");
 }

}
