import homepage from './homepage';
import activitiespage from './activityInfo';
import organizationProfile from './organizationProfile';
import orgpage from './OrganizationPage';

export const PlayVideo = (src) => homepage.playVideo(src);
export const FilterCarousel = () => homepage.filterCarousel();
export const LogoBanerCarousel = () => homepage.logoBanerCarousel();
export const SetUserLocation = () => homepage.setUserLocation();
export const GetUserLocation = () => homepage.getUserLocation();
export const IncreaseNumber = () => homepage.increaseNumber();
export const OtherAct = () => activitiespage.otherAct();
export const ShowResult = () => homepage.showResult();
export const InformationCarousel = () => organizationProfile.informationCarousel();
export const OrganizeCarousel = () => orgpage.organizeCarousel();