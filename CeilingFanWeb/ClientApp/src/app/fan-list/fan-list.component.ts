import { Component, OnInit } from '@angular/core';
import { Fan } from '../models/fan';
import { Subscription } from 'rxjs';
import { FanService } from '../services/fan.service';

@Component({
  selector: 'app-fan-list',
  templateUrl: './fan-list.component.html',
  styleUrls: ['./fan-list.component.css']
})
export class FanListComponent implements OnInit {

  /**
   * Fan list
   */
  fans: Array<Fan>;

  /**
   * A subscription so we can unsubscribe
   */
  fansServiceSubscription: Subscription;

  /**
   * Loading flag
   */
  loadingFans: boolean = false;

  /**
   * Error message
   */
  messages: string[] = [];

  constructor(private fanService: FanService) { }

  ngOnInit() {
    this.getFans();
  }

  /**
   * Dispose objects
   */
  ngOnDestroy() {
    this.fansServiceSubscription.unsubscribe();
  }


  /**
   * Get the fans
   */
  public getFans() {
    this.loadingFans = true;
    this.clearResult();
    this.fansServiceSubscription = this.fanService.getFans()
      .subscribe(fans => {
        this.fans = fans;
      },
        err => { this.messages.push(...err); }
    ).add(() => { this.loadingFans = false; });
  }



  /**
   * Clear
   */
  private clearResult() {
    this.messages = [];
    this.fans = [];
  }
}
