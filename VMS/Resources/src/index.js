import lodash from './common/lodash';
import homepage from './homepage';
import activitiespage from './activitiespage';

export const CloneDeep = (object) => lodash.CloneDeep(object);
export const PlayVideo = (src) => homepage.playVideo(src);
export const FilterCarousel = () => homepage.filterCarousel();
export const LogoBanerCarousel = () => homepage.logoBanerCarousel();
export const GetUserLocation = () => homepage.getUserLocation();
export const ShowResult = () => homepage.showResult();
export const OtherAct = () => activitiespage.otherAct();
