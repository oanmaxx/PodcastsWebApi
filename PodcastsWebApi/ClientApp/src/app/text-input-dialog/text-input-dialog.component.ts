import { Component, EventEmitter } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'text-input-dialog-component',
  templateUrl: './text-input-dialog.component.html'
})
export class TextInputDialogComponent {

  public title: string;
  public message: string;
  onOk = new EventEmitter();

  constructor(public dialog: MatDialogRef<TextInputDialogComponent>) { }
}
