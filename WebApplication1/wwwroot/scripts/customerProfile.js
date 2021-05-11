let urlParams = new URLSearchParams(window.location.search);
import GenericResultInterface from "/scripts/modules/genericResultInterface.js";

if (urlParams.get("id") == "me") {
  axios
    .get("https://localhost:44374/profile")
    .then((res) => res.data)
    .then((data) => {
      console.log(data);
      document.querySelector(".username").innerHTML += data.username;
      document.querySelector(".realName").innerHTML +=
        data.name + data.lastName;
      document.querySelector(".email").innerHTML += data.email;
      document.querySelector(".rating").innerHTML = starMethod(data.rating);
      document.querySelector(".address").innerHTML += data.address;
      document.querySelector(
        ".profilePicture"
      ).style.backgroundImage = `url(${data.profileImg})`;
      let customerId = { id: data.id, case: "myads" };
      let searchController = new GenericResultInterface(
        "customerAds",
        customerId
      );
      window.searchController = searchController;
    })
    .catch("im heree", console.log);
  document.querySelector(
    ".profilePicture"
  ).innerHTML += `<p data-toggle="modal" data-target="#changeProfilePicture" >
    📤
  </p>`;
  document.querySelector(
    ".header"
  ).innerHTML += ` <h3 data-toggle="modal" data-target="#changeSettingsModal"  data-placement="top" title="Tooltip on top">
  ⚙️
</h3>`;
} else {
  axios
    .get("https://localhost:44374/customer/" + urlParams.get("id"))
    .then((res) => res.data)
    .then((data) => {
      document.querySelector(".username").innerHTML += data.username;
      document.querySelector(".realName").innerHTML +=
        data.name + data.lastName;
      document.querySelector(".rating").innerHTML = starMethod(data.rating);
      document.querySelector(".email").innerHTML += data.email;
      document.querySelector(".address").innerHTML += data.address;
      document.querySelector(
        ".profilePicture"
      ).style.backgroundImage = `url(${data.profileImg})`;
      let customerId = { id: data.id };
      let searchController = new GenericResultInterface(
        "customerAds",
        customerId
      );
      window.searchController = searchController;
    });
}
