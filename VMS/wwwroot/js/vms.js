var vms;(()=>{"use strict";var e={d:(t,o)=>{for(var n in o)e.o(o,n)&&!e.o(t,n)&&Object.defineProperty(t,n,{enumerable:!0,get:o[n]})},o:(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r:e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}},t={};e.r(t),e.d(t,{FilterCarousel:()=>l,GetUserLocation:()=>d,IncreaseNumber:()=>g,LogoBanerCarousel:()=>c,OtherAct:()=>p,PlayVideo:()=>s,RankCarousel:()=>m,SetUserLocation:()=>u,SmoothScrollTo:()=>a});const o=e=>{clearTimeout(e.countUpTimeout),e._countUpOrigInnerHTML&&(e.innerHTML=e._countUpOrigInnerHTML,e._countUpOrigInnerHTML=void 0),e.style.visibility=""},n=(e,t={})=>{const{duration:o=1e3,delay:n=16}=t,r=o/n,i=e.toString().split(/(<[^>]+>|[0-9.][,.0-9]*[0-9]*)/),a=[];for(let e=0;e<r;e++)a.push("");for(let e=0;e<i.length;e++)if(/([0-9.][,.0-9]*[0-9]*)/.test(i[e])&&!/<[^>]+>/.test(i[e])){let t=i[e];const o=[...t.matchAll(/[.,]/g)].map((e=>({char:e[0],i:t.length-e.index-1}))).sort(((e,t)=>e.i-t.i));t=t.replace(/[.,]/g,"");let n=a.length-1;for(let e=r;e>=1;e--){let i=parseInt(t/r*e,10);i=o.reduce(((e,{char:t,i:o})=>e.length<=o?e:e.slice(0,-o)+t+e.slice(-o)),i.toString()),a[n--]+=i}}else for(let t=0;t<r;t++)a[t]+=i[e];return a[a.length]=e.toString(),a},r=e=>{var t={Latitude:e.coords.latitude,Longitude:e.coords.longitude};localStorage.setItem("UserLocation",JSON.stringify(t))},i=()=>{const e=e=>{new Waypoint({element:e,handler:function(){((e,t={})=>{const{action:r="start",duration:i=1e3,delay:a=16}=t;if("stop"===r)return void o(e);if(o(e),!/[0-9]/.test(e.innerHTML))return;const s=n(e.innerHTML,{duration:i||e.getAttribute("data-duration"),delay:a||e.getAttribute("data-delay")});e._countUpOrigInnerHTML=e.innerHTML,e.innerHTML=s[0]||"&nbsp;",e.style.visibility="visible";const l=function(){e.innerHTML=s.shift()||"&nbsp;",s.length?(clearTimeout(e.countUpTimeout),e.countUpTimeout=setTimeout(l,a)):e._countUpOrigInnerHTML=void 0};e.countUpTimeout=setTimeout(l,a)})(e),this.destroy()},offset:"bottom-in-view"})};$(document).ready((()=>{const t=document.querySelectorAll(".counter");[].forEach.call(t,e)}))},a=e=>(e=>{$("html, body").stop().animate({scrollTop:$(e).offset().top},500,"swing")})(e),s=e=>(e=>{const t=document.querySelector(".video-header__source");t.src=e,t.play()})(e),l=()=>{$(".filter__carousel").owlCarousel({loop:!0,margin:0,nav:!0,responsive:{0:{items:4,slideBy:4},1200:{items:6,slideBy:6}}})},c=()=>{$(".logoBaner__carousel").owlCarousel({loop:!0,margin:0,nav:!0,responsive:{0:{items:1}}})},u=()=>(()=>{if(!navigator.geolocation)return null;navigator.geolocation.getCurrentPosition(r)})(),d=()=>(()=>{const e=localStorage.getItem("UserLocation");return e?JSON.parse(e):null})(),g=()=>i(),p=()=>{$(".owl-carousel").owlCarousel({loop:!0,margin:10,nav:!0,navText:['<span class="material-icons">navigate_before</span>','<span class="material-icons">navigate_next</span>'],dots:!1,responsive:{0:{items:3,slideBy:4}}})},m=()=>{$(".rank__owlcrousel").owlCarousel({loop:!0,margin:0,nav:!0,autoplay:!0,autoplayTimeout:5e3,autoplaySpeed:1500,navSpeed:1500,dotsSpeed:1500,responsiveClass:!0,responsive:{0:{items:1,stagePadding:50},1200:{items:1,stagePadding:140},1400:{items:1,stagePadding:180}}})};vms=t})();