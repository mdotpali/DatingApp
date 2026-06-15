import {Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BusyService {

  busyInterceptor = signal(0);

  busy(){
    this.busyInterceptor.update(current => current +1);
  }

  idle() {
    this.busyInterceptor.update(current => Math.max(0, current -= 1));
  }


}
