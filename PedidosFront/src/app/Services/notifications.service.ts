import { Injectable} from '@angular/core';
import { Subject } from '../../../node_modules/rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  private pendingOrder:number = 0;
  private onChangePendingOrdersSource = new Subject<number>();
  public onChangePendingOrders$ = this.onChangePendingOrdersSource.asObservable();

  constructor() { }

  addPendingOrder(){
    this.pendingOrder ++;
    this.onChangePendingOrdersSource.next(this.pendingOrder);
  }





  
}
