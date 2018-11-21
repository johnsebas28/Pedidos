export class User {
    public idUser:String;
    public identification: String;
    public name:String;
    public lastName: String;
    public address: String;
    public nickName: String;
    public password: String;
    public email: String;
    public phone: String;
    public isActive : Boolean;
    constructor( ){
    this.idUser = "";
       this.address = "";
       this.email = "";
       this.identification = "";
       this.isActive = true;
       this.lastName = "";
       this.name = "";
       this.nickName = "";
       this.password = "";
       this.phone  = "";
    }
}
