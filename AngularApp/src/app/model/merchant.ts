import { Address } from "./address";
import { Card } from "./card";

export interface Merchant {
  name: string;
  email: string;
  password: string;
  cpf: string;
  cnpj: string;
  phone: string;
  address: Address
  flatId: string;
  card: Card;
}
