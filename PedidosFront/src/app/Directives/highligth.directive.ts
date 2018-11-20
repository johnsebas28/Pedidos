import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appHighligth]'
})
export class HighligthDirective {

  constructor(private el: ElementRef) {
   }

  @Input('appHighlight') highlightColor: string;

  @HostListener('mouseenter') onmouseenter(){
    this.el.nativeElement.style.backgroundColor='#298782';
  }
  

  @HostListener('mouseout') onmouseleave(){
    this.el.nativeElement.style.backgroundColor='#1F6764';
  }



}
