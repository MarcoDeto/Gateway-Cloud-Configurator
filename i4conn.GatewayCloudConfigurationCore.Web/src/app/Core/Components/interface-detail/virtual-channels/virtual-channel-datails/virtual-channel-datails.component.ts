import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { VirtualChannelsComponent } from '../virtual-channels.component';

@Component({
  selector: 'app-virtual-channel-datails',
  templateUrl: './virtual-channel-datails.component.html',
  styleUrls: ['./virtual-channel-datails.component.scss']
})
export class VirtualChannelDatailsComponent implements OnInit {

  subscribers: Subscription[] = [];
  rules = false;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<VirtualChannelsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
  }

  show($event: any) {
    if ($event == 1)
      this.rules = true;
  }

  getWidth(): number {
    return window.innerWidth;
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
