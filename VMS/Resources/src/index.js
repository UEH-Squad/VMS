import homepage from './homepage';
import activitiespage from './activityInfo';

export const PlayVideo = (src) => homepage.playVideo(src);
export const FilterCarousel = () => homepage.filterCarousel();
export const LogoBanerCarousel = () => homepage.logoBanerCarousel();
export const SetUserLocation = () => homepage.setUserLocation();
export const GetUserLocation = () => homepage.getUserLocation();
export const IncreaseNumber = () => homepage.increaseNumber();
export const OtherAct = (count) => activitiespage.otherAct(count);