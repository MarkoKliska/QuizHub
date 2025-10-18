export interface LoginRequestDto {
  identifier: string;
  password: string;
}

export interface LoginResponseDto {
  id: number;
  userName: string;
  email: string;
  role: string;
  token: string;
}