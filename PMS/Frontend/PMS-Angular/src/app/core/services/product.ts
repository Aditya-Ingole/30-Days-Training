import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../models/product';
import { CreateProduct } from '../models/create-product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}

  getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${environment.apiUrl}/products`);
  }

  createProduct(product: CreateProduct) {
    return this.http.post(
      `${environment.apiUrl}/products`,

      product,
    );
  }


  deleteProduct(id: number) {
    return this.http.delete(`${environment.apiUrl}/products/${id}`);
  }
  

  updateProduct(id: number, product: CreateProduct) {
    return this.http.put(
      `${environment.apiUrl}/products/${id}`,

      product,
    );
  }
}
