export interface Auth {
  authenticated: boolean,
  created: string,
  expiration: string
  accessToken: string,
  refreshToken: string;
  usertype: string;
}
