
// Collection of high-quality car images from Unsplash
const carImages = {
    // Brands
    'toyota': '/images/toyota.svg',
    'honda': '/images/honda.svg',
    'bmw': '/images/bmw.svg',
    'mercedes': '/images/luxury.svg',
    'ford': '/images/default.svg',
    'chevrolet': '/images/default.svg',
    'tesla': '/images/luxury.svg',
    'nissan': '/images/default.svg',

    // Categories
    'suv': '/images/default.svg',
    'luxury': '/images/luxury.svg',
    'economy': '/images/toyota.svg',
    'sports': '/images/bmw.svg',

    // Default Fallback
    'default': '/images/default.svg'
};

/**
 * Returns a car image URL based on the car's metadata.
 * Priority: 
 * 1. Existing imageUrl
 * 2. Match by Make (Toyota, BMW...)
 * 3. Match by Category (SUV, Luxury...)
 * 4. Default generic car
 */
export const getCarImage = (car) => {
    // Basic validation: must be string, decent length
    if (car.imageUrl && typeof car.imageUrl === 'string' && car.imageUrl.length > 15 && car.imageUrl.startsWith('http')) {
        return car.imageUrl;
    }

    const make = car.make ? car.make.toLowerCase() : '';
    // Check for exact make match
    for (const key of Object.keys(carImages)) {
        if (make.includes(key)) return carImages[key];
    }

    // Check category
    const category = typeof car.category === 'string' ? car.category.toLowerCase() : '';
    if (category.includes('suv')) return carImages['suv'];
    if (category.includes('luxury')) return carImages['luxury'];
    if (category.includes('economy')) return carImages['economy'];
    if (category.includes('sport')) return carImages['sports']; // 'sport' covers 'sports'

    // Fallback based on category ID if string was empty but we have int?
    // Not implemented here as category is usually mapped string by now.

    return carImages['default'];
};
