import { Address } from "./user"

export interface OrderToCreate {
    basketId: string;
    deliveryMethodId: number;
    shipToAddress: Address;
}

export interface Order {
    id: number
    buyerEmail: string
    orderDate: string
    shipToAdress: Address
    deliveryMethod: string
    shippingPrice: number
    orderItems: OrderItem[]
    subtotal: number
    total: number
    status: string
  }
  
  export interface ShipToAdress {
    firstName: string
    lastName: string
    street: string
    city: string
    state: string
    zipcode: string
  }
  
  export interface OrderItem {
    productId: number
    productName: string
    pictureUrl: string
    price: number
    quantity: number
  }