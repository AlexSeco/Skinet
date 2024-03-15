import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/models/order';
import { OrderService } from 'src/app/order/order.service';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit{

  order?: Order

  constructor(private orderService: OrderService, private route: ActivatedRoute, private bcService: BreadcrumbService) { }
  ngOnInit(): void {
    this.getOrderById()
  }

  getOrderById(){
    const id = this.route.snapshot.paramMap.get('id')
    id && this.orderService.getOrderById(+id).subscribe({
      next: order => {this.order = order;
      this.bcService.set('@OrderDetailed', `Order# ${order.id} - ${order.status}`)
      }
    })
  }

}
