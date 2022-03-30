import {AfterViewInit, Component, ContentChildren, ElementRef, OnInit, ViewChild} from '@angular/core';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent implements OnInit, AfterViewInit {
  @ContentChildren('btn') myVideo: any;

  constructor(private elem: ElementRef) {
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
  }

  // ngAfterViewInit(): void {
  //   let elements: NodeList = this.elem.nativeElement.querySelectorAll('.server-button');
  //   elements.forEach(element => {
  //     element.addEventListener("click", this.disableButton)
  //   })
  // }
  //
  // private disableButton(this: HTMLButtonElement) {
  //   this.disabled = true;
  //   setTimeout(()=>{this.disabled = false}, 3000);
  // }

}
