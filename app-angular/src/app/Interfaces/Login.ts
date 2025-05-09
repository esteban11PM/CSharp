export interface LoginRequest {
    username: string;
    password: string;
  }
  
  export interface LoginResponse {
    token: string;
    username: string;
    userId: number;
    rol: string;
  }
  