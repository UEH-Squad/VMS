import homepage from './homepage';
import organizationProfile from './organizationProfile';

export const PlayVideo = (src) => homepage.playVideo(src);
export const FilterCarousel = () => homepage.filterCarousel();
export const LogoBanerCarousel = () => homepage.logoBanerCarousel();
export const SetUserLocation = () => homepage.setUserLocation();
export const GetUserLocation = () => homepage.getUserLocation();
export const ShowResult = () => homepage.showResult();
export const IncreaseNumber = () => homepage.increaseNumber();
export const InformationCarousel = () => organizationProfile.informationCarousel();