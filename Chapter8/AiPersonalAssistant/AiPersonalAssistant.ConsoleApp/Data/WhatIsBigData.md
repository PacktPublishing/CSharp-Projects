---
title: What is Big Data?
lead: Exploring the idea of Big Data and the 5 V's that go with it.
summary: Big data refers to data problems so massive that a human doesn’t have much of a chance of keeping up and must rely on computer assistance due to the scale of the data problems they’re dealing with.
author: "Matt Eland"
tags: [Concepts]
draft: false
date: "2022-08-29"
thumbnail: ../../img/BigData/banner.png
showThumbnail: true
---

**Big data** refers to data problems so massive that a human doesn’t have much of a chance of keeping up and must rely on computer assistance due to the scale of the data problems they’re dealing with.

## The Five V's of Big Data

We can define big data problems in terms of five keywords that all happen to start with the letter `V` in English. These are commonly referred to as *"the five V's"* and are as follows:

- **Velocity** – the rate at which data arrives
- **Variety** – the large amount of different forms data can take on
- **Veracity** – the difficulty which we may have trying to determine the reliability of the data
- **Volume** – the sheer quantity of the data
- **Value** – deriving benefit to others from the data by determining relationships in the data

These five V's are easy to deal with at smaller applications, but when one or more of these becomes difficult with a given data problem, you're probably dealing with big data.

## A Chatbot Example

An example of a big data problem from things I’m working on at the moment would be a problem I’m facing in conversational AI development.

I'm building a [chatbot](https://accessibleai.dev/chat) to support the community in learning data science concepts, but the variety of things people might say to the bot poses some training difficulties. This *variety* might not just be limited to text queries but may include images / screenshots and emoji, or even queries in different languages.

Since the world is so big, it's possible that many people suddenly discover my site and I would now have a *velocity* problem with too many requests coming in for a human to handle and something that needed an automated system.

I might choose to use sources from the internet or social media posts to train my chatbot, but now I have a *volume* problem because there’s so much out there of questionable *veracity* and now I need to be able to sift through the noise to find answers of *value* to people trying to interact with my chatbot.

## Conclusion

Big Data is simply a term we use to refer to data problems of sufficient scale that a human is woefully unprepared to tackle them alone.

We need specialized tools and techniques to solve big data problems. This is key to the growing prominence of technologies such as [machine learning](https://accessibleai.dev/post/aivsml/), data mining, horizontally-scalable databases, and distributed processing.