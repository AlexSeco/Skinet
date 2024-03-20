import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DeliveryMethods } from '../models/deliveryMethods';
import { map } from 'rxjs';
import { Address } from '../models/user';
import { Order, OrderToCreate } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }


  getDeliveryMethods() {
    return this.http.get<DeliveryMethods[]>(this.baseUrl + 'orders/deliveryMethods').pipe(
      map(dm => {
        return dm.sort((a, b) => b.price - a.price)
      })
    )
  }

  createOrder(order: OrderToCreate){
    return this.http.post<Order>(this.baseUrl + 'orders', order);
  }

}
