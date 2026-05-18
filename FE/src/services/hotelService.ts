import { getAuthToken } from "./authService";

export type HotelRoom = {
  id: number;
  roomNumber?: string;
};

export type Hotel = {
  id: number;
  name: string;
  address: string;
  description?: string | null;
  mainImageUrl?: string | null;
  imageGallery?: string[] | null;
  hostId: number;
  rooms?: HotelRoom[] | null;
};

export type HotelPayload = {
  name: string;
  address: string;
  description?: string;
  mainImageUrl?: string;
  imageGallery: string[];
};

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5096";

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
    throw new Error(data?.message ?? "Không thể thao tác khách sạn");
  }

  return data as T;
}

export function getHotels() {
  return request<Hotel[]>("/api/hotels");
}

export function getHotelById(id: number) {
  return request<Hotel>(`/api/hotels/${id}`);
}

export function createHotel(payload: HotelPayload) {
  return request<Hotel>("/api/hotels", {
    method: "POST",
    body: JSON.stringify(payload),
  });
}

export function updateHotel(id: number, payload: HotelPayload) {
  return request<void>(`/api/hotels/${id}`, {
    method: "PUT",
    body: JSON.stringify(payload),
  });
}

export function deleteHotel(id: number) {
  return request<void>(`/api/hotels/${id}`, {
    method: "DELETE",
  });
}
