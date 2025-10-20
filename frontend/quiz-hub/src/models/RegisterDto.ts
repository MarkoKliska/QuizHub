export interface RegisterUserRequestDto {
  username: string;
  email: string;
  password: string;
  pictureBase64?: string;
}

export interface RegisterUserResponseDto {
  id: number;
  username: string;
  email: string;
  token: string;
}