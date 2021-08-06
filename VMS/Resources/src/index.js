import lodash from './common/lodash';
import homepage from './homepage'

export const CloneDeep = (object) => lodash.CloneDeep(object);
export const Slick = (slidesToShow, slidesToScroll, isInfinite) => homepage.slick(slidesToShow, slidesToScroll, isInfinite);
export const PlayVideo = (src) => homepage.playVideo(src);
export const GetUserLocation = () => homepage.getUserLocation();