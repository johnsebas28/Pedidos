import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { User } from '../../Models/User';
import { OrderHttpService } from '../../Services/order-http.service';
import {MatPaginator,MatTableDataSource, MatSort, MatDialog} from '@angular/material';
import {SelectionModel} from '@angular/cdk/collections';
import { ModalUserComponent } from './modal/modal-user/modal-user.component';
import { NotificationsService } from '../../Services/notifications.service';


@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
  providers:[OrderHttpService]
})

export class UserComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  
  controller:string;
  model:User;
  arrayUser: User[];
  displayedColumns: string[] = ['select', 'identification', 'name', 'lastName'];
  dataSource: MatTableDataSource<User>;
  selection = new SelectionModel<User>(true, [],true);
  isSelected:false;
  idUser:number;
  arrayUSerSelected: Array<String>;

  constructor( 
  private http: OrderHttpService,
  public dialog: MatDialog,
  private notificationService: NotificationsService ) 
  { 
      this.model = new User();
      this.idUser = 0;
  }

 
ngOnInit() {
    this.arrayUSerSelected = new Array<String>();
    this.controller = "user";
    this.LoadTable();
      // selection changed
    this.selection.onChange.subscribe((a) =>
    {
      
        if (a.added[0])   // will be undefined if no selection
        {
            // alert('You selected ' + a.added[0].identification);

            this.model = a.added[0];
            this.arrayUSerSelected.push( a.added[0].idUser);
        }
       
        if(a.removed[0]){
          var index = this.arrayUSerSelected.indexOf(a.removed[0].idUser);
          if (index > -1) {
            this.arrayUSerSelected.splice(index, 1);
          }
        }

        console.log( this.arrayUSerSelected);
    });
  }

  private LoadTable() {
    this.http.get("https://localhost:44371/api/"+ this.controller, null).subscribe((res: any) => {
      console.log(res);
      this.arrayUser = res.lsdata;
      this.dataSource = new MatTableDataSource(res.lsdata);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }, err => {
      console.log("Error en la respuesta");
    });
  }

  newUser(){
    this.model=new User();
    const dialogRef = this.dialog.open(ModalUserComponent, {
      width: '950px',
      data: this.model
    });

    dialogRef.afterClosed().subscribe((result:User) => {
     
      if(result !== null){
        let index:number;
        index = 0;
        this.arrayUser.forEach( (item, index) => {
          if(item.identification === result.identification && item.nickName === result.nickName){
            this.arrayUser.splice(index,1);
          } 
        });
        this.arrayUser.push(result);
        this.arrayUser.sort((a, b) => a.name < b.name ? -1 : a.name > b.name ? 1 : 0)
        this.dataSource = new MatTableDataSource(this.arrayUser);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    });
  }

  
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }


  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  deleteUser(){
    if(this.selection.hasValue()){
      for (let i = 0; i < this.arrayUSerSelected.length; i++) {
        this.http.delete("https://localhost:44371/api/"+ this.controller,this.arrayUSerSelected[i], null).subscribe((res: any) => {
          console.log(res);

         this.LoadTable();
        }, err => {
          console.log("Error en la respuesta");
        });
      }
    }
  }
  openDialog(identification:string): void {
    let newModel = new User();
    newModel = this.arrayUser.filter(x=> x.identification === identification)[0];
    //const newmodelCopy = Object.assign({}, newModel);
    const newmodelCopy = {...newModel};

    const dialogRef = this.dialog.open(ModalUserComponent, {
      width: '950px',
      data: newmodelCopy
    });

    dialogRef.afterClosed().subscribe((result:User) => {
      if(result !== null){
        this.arrayUser.forEach( (item, index) => {
          if(item.identification === result.identification && item.nickName === result.nickName){
            this.arrayUser.splice(index,1);
          } 
        });
        this.arrayUser.push(result);
        this.arrayUser.sort((a, b) => a.name < b.name ? -1 : a.name > b.name ? 1 : 0)
        this.dataSource = new MatTableDataSource(this.arrayUser);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    });
  }
  PushOrder(){
    this.notificationService.addPendingOrder();
  }
}
