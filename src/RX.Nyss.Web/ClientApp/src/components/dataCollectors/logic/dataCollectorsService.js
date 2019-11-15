export const getBirthDecades = () => {
  const yearMax = Math.floor(new Date().getFullYear() / 10) * 10;
  return Array.from({ length: 10 }, (_, i) => String(yearMax - (10 * i)));
}