import { API_BASE_URL } from '../config/apiConfig';
import { getAuthToken } from "./authService";

export type HotelDto = {
  id: number;
  name: string;
  address: string;
};

export type UserRecord = {
  id: number;
  username: string;
  email?: string | null;
  role?: string | null;
  hotels: HotelDto[];
};

export type UserPayload = {
  username: string;
  password?: string;
  email?: string;
  role?: string;
};



async function request<T>(path: string, init: RequestInit = {}): Promise<T> {
  const token = getAuthToken();
  const headers = new Headers(init.headers);

  if (!headers.has("Content-Type") && init.body) {
    headers.set("Content-Type", "application/json");
  }

  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...init,
    headers,
  });

  if (response.status === 204) {
    return undefined as T;
  }

  const data = await response.json().catch(() => ({}));

  if (!response.ok) {
    throw new Error(data?.message ?? "Không thể thao tác tài khoản");
  }

  return data as T;
}

export function getUsers() {
  return request<UserRecord[]>("/api/users");
}

export function getUserById(id: number) {
  return request<UserRecord>(`/api/users/${id}`);
}

export function createUser(payload: UserPayload) {
  return request<{ message?: string; data?: UserRecord }>("/api/users", {
    method: "POST",
    body: JSON.stringify(payload),
  });
}

export function updateUser(id: number, payload: UserPayload) {
  return request<{ message?: string }>(`/api/users/${id}`, {
    method: "PUT",
    body: JSON.stringify(payload),
  });
}

export function deleteUser(id: number) {
  return request<{ message?: string }>(`/api/users/${id}`, {
    method: "DELETE",
  });
}
