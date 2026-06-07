import { API_BASE_URL } from '../config/apiConfig';

export interface PricingFeature {
    name: string;
    value: string;
    highlight?: boolean;
    disabled?: boolean;
}

export interface PricingPlan {
    id: number;
    name: string;
    subtitle: string;
    price: string;
    features: PricingFeature[];
    btnText: string;
    isPopular: boolean;
}

export const getPricingPlans = async (): Promise<PricingPlan[]> => {
    try {
        const response = await fetch(`${API_BASE_URL}/api/pricing-plans`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        return data.data;
    } catch (error) {
        console.error("Error fetching pricing plans:", error);
        throw error;
    }
};

export const createPricingPlan = async (planData: Omit<PricingPlan, "id">, token: string): Promise<any> => {
    try {
        const response = await fetch(`${API_BASE_URL}/api/pricing-plans`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(planData)
        });
        if (!response.ok) {
            const err = await response.json().catch(() => ({}));
            throw new Error(err.message || `HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error("Error creating pricing plan:", error);
        throw error;
    }
};

export const updatePricingPlan = async (id: number, planData: Omit<PricingPlan, "id">, token: string): Promise<any> => {
    try {
        const response = await fetch(`${API_BASE_URL}/api/pricing-plans/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(planData)
        });
        if (!response.ok) {
            const err = await response.json().catch(() => ({}));
            throw new Error(err.message || `HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error("Error updating pricing plan:", error);
        throw error;
    }
};

export const deletePricingPlan = async (id: number, token: string): Promise<any> => {
    try {
        const response = await fetch(`${API_BASE_URL}/api/pricing-plans/${id}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });
        if (!response.ok) {
            const err = await response.json().catch(() => ({}));
            throw new Error(err.message || `HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error("Error deleting pricing plan:", error);
        throw error;
    }
};
